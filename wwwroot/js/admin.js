
//Hambuger menüsü

const sidebar = document.getElementById("sidebar");
const hamburgerMenu = document.getElementById("hamburger-menu");

// Hamburger menü tıklama olayı
hamburgerMenu.addEventListener("click", () => {
    sidebar.classList.toggle("active");
});
//Hambuger menüsü bitiş
