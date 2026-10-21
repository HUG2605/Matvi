class ThemeLayout {
    constructor() {
        this.html = document.getElementsByTagName("html")[0];

        $(() => {
            $('[data-plugin="knob"]').knob();
        });
    }

    initComponents() {
        Waves.init();
        lucide.createIcons();

        [...document.querySelectorAll('[data-bs-toggle="popover"]')]
            .map(el => new bootstrap.Popover(el));
        [...document.querySelectorAll('[data-bs-toggle="tooltip"]')]
            .map(el => new bootstrap.Tooltip(el));
        [...document.querySelectorAll(".offcanvas")]
            .map(el => new bootstrap.Offcanvas(el));

        const toastPlacement = document.getElementById("toastPlacement");
        if (toastPlacement) {
            document.getElementById("selectToastPlacement")
                .addEventListener("change", function () {
                    toastPlacement.dataset.originalClass ||= toastPlacement.className;
                    toastPlacement.className = `${toastPlacement.dataset.originalClass} ${this.value}`;
                });
        }

        [...document.querySelectorAll(".toast")]
            .map(el => new bootstrap.Toast(el));

        const alertPlaceholder = document.getElementById("liveAlertPlaceholder");
        const alertBtn = document.getElementById("liveAlertBtn");

        if (alertBtn) {
            alertBtn.addEventListener("click", () => {
                const message = "Nice, you triggered this alert message!";
                const type = "success";
                const wrapper = document.createElement("div");

                wrapper.innerHTML = [
                    `<div class="alert alert-${type} alert-dismissible" role="alert">`,
                    `   <div>${message}</div>`,
                    '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
                    "</div>"
                ].join("");

                alertPlaceholder.append(wrapper);
            });
        }

        let themeToggle = document.getElementById("theme-mode");
        if (themeToggle) {
            themeToggle.addEventListener("click", () => {
                const html = document.documentElement;
                const next = html.getAttribute("data-bs-theme") === "light" ? "dark" : "light";
                html.setAttribute("data-bs-theme", next);
                sessionStorage.setItem("themeMode", next);
            });
        }

        const userPref = sessionStorage.getItem("themeMode");
        const sysPref = window.matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light";
        const finalTheme = userPref || sysPref;

        document.documentElement.setAttribute("data-bs-theme", finalTheme);
        sessionStorage.setItem("themeMode", finalTheme);
    }

    initfullScreenListener() {
        const toggleBtn = document.querySelector('[data-bs-toggle="fullscreen"]');
        if (!toggleBtn) return;

        toggleBtn.addEventListener("click", e => {
            e.preventDefault();
            document.body.classList.toggle("fullscreen-enable");

            const doc = document;
            if (doc.fullscreenElement || doc.mozFullScreenElement || doc.webkitFullscreenElement) {
                doc.cancelFullScreen?.();
                doc.mozCancelFullScreen?.();
                doc.webkitCancelFullScreen?.();
            } else {
                doc.documentElement.requestFullscreen?.();
                doc.documentElement.mozRequestFullScreen?.();
                doc.documentElement.webkitRequestFullscreen?.(Element.ALLOW_KEYBOARD_INPUT);
            }
        });
    }

    initFormValidation() {
        document.querySelectorAll(".needs-validation").forEach(form => {
            form.addEventListener("submit", e => {
                if (!form.checkValidity()) {
                    e.preventDefault();
                    e.stopPropagation();
                }
                form.classList.add("was-validated");
            }, false);
        });
    }

    initMainMenu() {
        if (!$(".app-menu").length) return;

        const collapses = $(".app-menu .menu-item .collapse");
        $(".app-menu li [data-bs-toggle='collapse']").on("click", () => false);

        collapses.on({
            "show.bs.collapse": function (e) {
                const parentsShown = $(e.target).parents(".collapse.show");
                $(".app-menu .collapse.show").not(e.target).not(parentsShown).collapse("hide");
            }
        });

        const links = document.querySelectorAll(".app-menu .menu-link");
        const current = window.location.href.split(/[?#]/)[0];

        links.forEach(link => {
            if (link.href === current) {
                link.classList.add("active");
                link.parentNode.classList.add("active");
                link.parentNode.parentNode.parentNode.classList.add("show");
                link.parentNode.parentNode.parentNode.parentNode.classList.add("active");
                link.parentNode.parentNode.parentNode.parentNode.parentNode.classList.add("active");
                link.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.classList.add("show");
            }
        });

        setTimeout(() => {
            const activeItem = document.querySelector("li.active .active");
            if (!activeItem) return;

            const container = document.querySelector(".main-menu .simplebar-content-wrapper");
            const scrollDistance = activeItem.offsetTop - 300;

            if (container && scrollDistance > 100) {
                let start = container.scrollTop;
                let change = scrollDistance - start;
                let duration = 600;
                let currentTime = 0;

                function animateScroll() {
                    currentTime += 20;
                    let val = currentTime / duration / 2;
                    let next = val < 1
                        ? change / 2 * val * val + start
                        : -change / 2 * (--val * (val - 2) - 1) + start;
                    container.scrollTop = next;

                    if (currentTime < duration) setTimeout(animateScroll, 20);
                }

                animateScroll();
            }
        }, 200);
    }

    reverseQuery(el, selector) {
        while (el) {
            if (el.parentElement && el.parentElement.querySelector(selector) === el) return el;
            el = el.parentElement;
        }
        return null;
    }

    initSwitchListener() {
        const toggleMenuBtn = document.querySelector(".button-toggle-menu");
        if (!toggleMenuBtn) return;

        toggleMenuBtn.addEventListener("click", () => {
            if (this.html.getAttribute("data-sidebar-size") === "full") {
                this.showBackdrop();
            }
            this.html.classList.toggle("sidebar-enable");
        });
    }

    showBackdrop() {
        const scrollbarWidth = (() => {
            const temp = document.createElement("div");
            temp.style.width = "100px";
            temp.style.height = "100px";
            temp.style.overflow = "scroll";
            document.body.appendChild(temp);
            const width = temp.offsetWidth - temp.clientWidth;
            document.body.removeChild(temp);
            return width;
        })();

        const backdrop = document.createElement("div");
        backdrop.id = "custom-backdrop";
        backdrop.classList = "offcanvas-backdrop fade show";

        document.body.appendChild(backdrop);
        document.body.style.overflow = "hidden";
        document.body.style.paddingRight = `${scrollbarWidth}px`;

        backdrop.addEventListener("click", () => {
            this.html.classList.remove("sidebar-enable");
            this.hideBackdrop();
        });
    }

    hideBackdrop() {
        const backdrop = document.getElementById("custom-backdrop");
        if (backdrop) {
            document.body.removeChild(backdrop);
            document.body.style.overflow = null;
            document.body.style.paddingRight = null;
        }
    }

    init() {
        this.initComponents();
        this.initfullScreenListener();
        this.initFormValidation();
        this.initMainMenu();
        this.initSwitchListener();
    }
}

// Inicializaci�n visual global
(new ThemeLayout).init();