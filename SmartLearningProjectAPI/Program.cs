
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ITIEntity>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ITIEntity>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddAutoMapper(typeof(InstructorProfile));
builder.Services.AddAutoMapper(typeof(StudentProfile));

builder.Services.AddScoped<IStudentService, StudentServices>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddAutoMapper(typeof(CourseProfile));
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddAutoMapper(typeof(UnitProfile));

builder.Services.AddScoped<ILessonService, LessonsService>();
builder.Services.AddAutoMapper(typeof(LessonsProfile));
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddAutoMapper(typeof(ResourceProfile));

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();



//---- External Services
builder.Services.AddHttpClient<IChatGPTService, ChatGPTService>();
builder.Services.AddScoped<IPdfChatService, PdfChatService>();

builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddAutoMapper(typeof(MeetingProfile));

builder.Services.AddScoped<IStreamTokenService, StreamTokenService>();

//---- End External Services

var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

Console.WriteLine($"Secret: {jwtSettings["Secret"]}");
Console.WriteLine($"Issuer: {jwtSettings["Issuer"]}");
Console.WriteLine($"Audience: {jwtSettings["Audience"]}");

// [Authoriz] Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Demo API",
        Description = "ITI Project"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] then your token.\r\n\r\nExample: \"Bearer eyJhbGciOi...\""
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
});

builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            //policy.WithOrigins("http://localhost:4200", "http://localhost:5693")
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
                  //.AllowCredentials();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);


app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

//app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
