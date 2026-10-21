(function () {
    class MVStepper {
        constructor(selector, options = {}) {
            this.container = document.querySelector(selector);
            this.steps = Array.from(this.container.querySelectorAll('.mv-step-vertical'));
            this.currentStep = 0;

            this.form = options.formSelector ? document.querySelector(options.formSelector) : null;
            this.onStepChange = options.onStepChange || function () { };
            this.validateStep = options.validateStep || null;

            // Botones
            this.btnPrev = document.querySelector(options.buttonSelectors?.previous);
            this.btnNext = document.querySelector(options.buttonSelectors?.next);
            this.btnSubmit = document.querySelector(options.buttonSelectors?.submit);

            this.init();
        }

        init() {
            this.updateVisualState();
            this.showCollapse(this.currentStep);
            this.updateButtons();

            this.btnNext?.addEventListener('click', () => this.nextStep());
            this.btnPrev?.addEventListener('click', () => this.prevStep());
        }

        updateVisualState() {
            this.steps.forEach((step, index) => {
                step.classList.remove('active', 'complete');
                if (index < this.currentStep) {
                    step.classList.add('complete');
                } else if (index === this.currentStep) {
                    step.classList.add('active');
                }
            });
        }

        showCollapse(index) {
            this.steps.forEach((step, i) => {
                const collapse = step.querySelector('.collapse');
                if (collapse) {
                    const bsCollapse = bootstrap.Collapse.getOrCreateInstance(collapse, { toggle: false });
                    i === index ? bsCollapse.show() : bsCollapse.hide();
                }
            });
        }

        updateButtons() {
            const isFirst = this.currentStep === 0;
            const isLast = this.currentStep === this.steps.length - 1;

            if (this.btnPrev) this.btnPrev.disabled = isFirst;
            if (this.btnNext) this.btnNext.disabled = isLast;

            if (this.btnSubmit) {
                this.btnSubmit.classList.toggle('d-none', !isLast);
            }
        }

        validateCurrentStep() {
            if (!this.form) return true;
            const currentCollapse = this.steps[this.currentStep].querySelector('.collapse');
            const inputs = currentCollapse ? currentCollapse.querySelectorAll('input, select, textarea') : [];
            let valid = true;

            inputs.forEach(input => {
                if (!$(input).valid()) valid = false;
            });

            return valid;
        }

        nextStep() {
            if (!this.validateCurrentStep()) return;
            if (this.validateStep && !this.validateStep(this.currentStep)) return;
            if (this.currentStep < this.steps.length - 1) {
                this.currentStep++;
                this.updateVisualState();
                this.showCollapse(this.currentStep);
                this.updateButtons();
                this.onStepChange(this.currentStep);
            }
        }

        prevStep() {
            if (this.currentStep > 0) {
                this.currentStep--;
                this.updateVisualState();
                this.showCollapse(this.currentStep);
                this.updateButtons();
                this.onStepChange(this.currentStep);
            }
        }
    }

    window.MVStepper = MVStepper;
})();
