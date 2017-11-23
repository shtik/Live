namespace Shtik.Nav {
    interface IPartial {
        slideImageUrl: string;
    }

    function loadSlide(url: string) {
        return fetch(url, { method: "GET" })
            .then(response => response.ok ? response.text() : null);
    }


    export class NavButtons {
        private _first: HTMLButtonElement;
        private _previous: HTMLButtonElement;
        private _next: HTMLButtonElement;
        private _last: HTMLButtonElement;
        private _image: HTMLDivElement;

        constructor() {
            this._first = document.querySelector("button#first-btn") as HTMLButtonElement;
            this._first.addEventListener("click", this._onFirst);

            this._previous = document.querySelector("button#previous-btn") as HTMLButtonElement;
            this._previous.addEventListener("click", this._onPrevious);

            this._next = document.querySelector("button#next-btn") as HTMLButtonElement;
            this._next.addEventListener("click", this._onNext);

            this._last = document.querySelector("button#last-btn") as HTMLButtonElement;
            this._last.addEventListener("click", this._onLast);

            this._image = document.querySelector("div#slide-image") as HTMLDivElement;

            window.addEventListener("popstate", this.transition);
        }

        public transition = () => {
            const url = window.location.href + "/partial";
            loadSlide(url)
                .then(json => {
                    if (!json) {
                        this.goBack();
                        return;
                    }
                    const partial = JSON.parse(json) as IPartial;
                    this._image.style.backgroundImage = `url(${partial.slideImageUrl})`;
                });
        }

        public go = (href) => {
            history.pushState(null, null, href);
            this.transition();
        }

        public goBack =() => {
            const parts = location.pathname.split("/");
            const slide = Math.max(parseInt(parts.pop()) - 1, 0);
            const href = location.href.replace(/\/[0-9]+$/, `/${slide}`);
            this.go(href);
        }

        private _onFirst = () => {
            const current = this._currentSlide();
            if (current.slide === 0) return;
            const href = `/live/${current.presenter}/${current.slug}/0`;
            this.go(href);
        }

        private _onLast = () => {
            const current = this._currentSlide();
            const href = `/live/${current.presenter}/${current.slug}`;
            history.pushState(null, null, href);
            window.location.reload();
        }

        private _onPrevious = () => {
            const current = this._currentSlide();
            if (current.slide === 0) return;
            const href = `/live/${current.presenter}/${current.slug}/${current.slide - 1}`;
            this.go(href);
        }

        private _onNext = () => {
            const current = this._currentSlide();
            const href = `/live/${current.presenter}/${current.slug}/${current.slide + 1}`;
            this.go(href);
        }

        private _currentSlide = () => {
            const parts = location.pathname.split("/");
            const slide = parseInt(parts.pop());
            const slug = parts.pop();
            const presenter = parts.pop();
            return { presenter, slug, slide };
        }
    }
}