document.addEventListener("DOMContentLoaded", function () {
    const hamburger = document.getElementById("hamburger");
    const sidebar = document.getElementById("sidebar");
    const mainContent = document.getElementById("main-content");
    const preloader = document.getElementById("preloader");

    // Sidebar aç gapa
    function toggleSidebar() {
        sidebar.classList.toggle("active");
        if (sidebar.classList.contains("active")) {
            mainContent.style.marginLeft = "250px";
        } else {
            mainContent.style.marginLeft = "0";
        }
    }

    // Hamburger 
    hamburger.addEventListener("click", function (event) {
        toggleSidebar();
        event.stopPropagation();
    });

    
    document.addEventListener("click", function (event) {
        if (sidebar.classList.contains("active")) {
            toggleSidebar();
        }
    });

    // Sidebara tıklanırsa sayfa tıklama olayını engelle
    sidebar.addEventListener("click", function (event) {
        event.stopPropagation();
    });

    // Ekran boyutu değiştiğinde sidebarı sıfırla
    window.addEventListener("resize", function () {
        if (window.innerWidth > 1200) {
            sidebar.classList.remove("active");
            mainContent.style.marginLeft = "250px";
        } else {
            sidebar.classList.remove("active");
            mainContent.style.marginLeft = "0";
        }
    });

    // Soktuğumun prelqaderı buna ihtiyaç var mı yok mu emin değilim ama yaptım 
    window.addEventListener("load", function () {
        preloader.style.display = "none";
        document.body.style.overflow = "auto"; // Sayfa kaydırmasını aktif hale getir şimdilik bunu mobil uyumlu yapmam lazım
    });
});