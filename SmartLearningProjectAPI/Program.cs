


using SmartLearning.Application.Interfaces;
using SmartLearning.Application.Mappings;
using SmartLearning.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ITIEntity>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ITIEntity>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddAutoMapper(typeof(InstructorProfile));

// course 
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddAutoMapper(typeof(CourseProfile));
// Unit
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddAutoMapper(typeof(UnitProfile));

// Lesson
builder.Services.AddScoped<ILessonService, LessonsService>();
builder.Services.AddAutoMapper(typeof(LessonsProfile));



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
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudiance"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<TransactionMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
