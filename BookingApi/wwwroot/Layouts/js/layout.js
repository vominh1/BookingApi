
async function loadLayout() {
    const header = await fetch("/layouts/header.html").then(r => r.text());
    const sidebar = await fetch("/layouts/sidebar.html").then(r => r.text());
    const footer = await fetch("/layouts/footer.html").then(r => r.text());

    document.getElementById("header").innerHTML = header;
    document.getElementById("sidebar").innerHTML = sidebar;
    document.getElementById("footer").innerHTML = footer;
}

document.addEventListener("DOMContentLoaded", loadLayout);
