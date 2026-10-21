class Config {
    adjustLayout() {
        const html = document.getElementsByTagName("html")[0];
        if (window.innerWidth <= 1200) {
            html.setAttribute("data-sidebar-size", "full");
        }
    }

    initWindowSize() {
        const self = this;
        window.addEventListener("resize", function () {
            self.adjustLayout();
        });
    }

    init() {
        this.adjustLayout();
        this.initWindowSize();
    }
}

// Inicializar configuración de layout responsivo
(new Config).init();