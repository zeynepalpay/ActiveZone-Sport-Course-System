using Microsoft.EntityFrameworkCore;
using SportCourseRegistrationSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC yapısının çalışması için gerekli servisleri ekliyoruz
builder.Services.AddControllersWithViews();

// Veri tabanı bağlantımızı (DbContext) ve SQLite ayarımızı sisteme tanıtıyoruz
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(20); // 20 dakika işlem yapılmazsa oturum düşer
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Layout içinde session kontrolü yapabilmek için bu servisi kaydediyoruz
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Tarayıcıdaki hata sayfaları ve HTTPS yönlendirme ayarları
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Proje ilk açıldığında hangi sayfanın geleceğini belirleyen rota (Route) ayarı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
