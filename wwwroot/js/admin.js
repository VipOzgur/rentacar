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

//evet hayır kutucuğu queryselector ile seçip aktive kılassını ekliyo 
function setActive(value) {
    const input = document.getElementById("isActiveInput");
    const yesButton = document.querySelector(".btn-yes");
    const noButton = document.querySelector(".btn-no");
    input.value = value; 
    if (value) {
        yesButton.classList.add("active");
        noButton.classList.remove("active");
    } else {
        noButton.classList.add("active");
        yesButton.classList.remove("active");
    }
}


document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

    var calendar = new FullCalendar.Calendar(calendarEl, {
      initialView: 'dayGridMonth',
      headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
      },
      events: [
        { title: 'Araç Bakımı', start: '2025-01-23', end: '2025-01-25' },
        { title: 'Rezervasyon Teslimi', start: '2025-01-20' },
        { title: 'Teslim Alma', start: '2025-01-26', allDay: true }
      ]
    });

    calendar.render();
  });