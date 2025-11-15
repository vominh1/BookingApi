    async function loadLayout() {
            try {
                // Load header
                const headerResponse = await fetch("/layouts/header.html");
    const headerHTML = await headerResponse.text();
    document.getElementById("header").innerHTML = headerHTML;

    // Load sidebar
    const sidebarResponse = await fetch("/layouts/sidebar.html");
    const sidebarHTML = await sidebarResponse.text();
    document.getElementById("sidebar").innerHTML = sidebarHTML;

    // Load footer
    const footerResponse = await fetch("/layouts/footer.html");
    const footerHTML = await footerResponse.text();
    document.getElementById("footer").innerHTML = footerHTML;

    // Highlight active menu item based on current page
    highlightActiveMenu();
            } catch (error) {
        console.error("Error loading layouts:", error);
            }
        }

    function highlightActiveMenu() {
            const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.nav-link');
            
            navLinks.forEach(link => {
                if (link.getAttribute('href') === currentPath) {
        link.classList.add('active');
                }
            });
        }

    // Load layouts when DOM is ready
    document.addEventListener("DOMContentLoaded", loadLayout);