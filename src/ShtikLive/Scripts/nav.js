var Shtik;
(function (Shtik) {
    var Nav;
    (function (Nav) {
        function loadSlide(url) {
            return fetch(url, { method: "GET" })
                .then(response => response.ok ? response.text() : null);
        }
        class NavButtons {
            constructor() {
                this.transition = () => {
                    const url = window.location.href + "/partial";
                    loadSlide(url)
                        .then(json => {
                        if (!json) {
                            this.goBack();
                            return;
                        }
                        const partial = JSON.parse(json);
                        const article = document.querySelector("article#slide");
                        article.innerHTML = partial.html;
                        article.className = partial.layout;
                    });
                };
                this.go = (href) => {
                    history.pushState(null, null, href);
                    this.transition();
                };
                this.goBack = () => {
                    const parts = location.pathname.split("/");
                    const slide = Math.max(parseInt(parts.pop()) - 1, 0);
                    const href = location.href.replace(/\/[0-9]+$/, `/${slide}`);
                    this.go(href);
                };
                this._onFirst = () => {
                    const current = this._currentSlide();
                    if (current.slide === 0)
                        return;
                    const href = `/live/${current.presenter}/${current.slug}/0`;
                    this.go(href);
                };
                this._onLast = () => {
                    const current = this._currentSlide();
                    const href = `/live/${current.presenter}/${current.slug}`;
                    history.pushState(null, null, href);
                    window.location.reload();
                };
                this._onPrevious = () => {
                    const current = this._currentSlide();
                    if (current.slide === 0)
                        return;
                    const href = `/live/${current.presenter}/${current.slug}/${current.slide - 1}`;
                    this.go(href);
                };
                this._onNext = () => {
                    const current = this._currentSlide();
                    const href = `/live/${current.presenter}/${current.slug}/${current.slide + 1}`;
                    this.go(href);
                };
                this._currentSlide = () => {
                    const parts = location.pathname.split("/");
                    const slide = parseInt(parts.pop());
                    const slug = parts.pop();
                    const presenter = parts.pop();
                    return { presenter, slug, slide };
                };
                this._first = document.querySelector("button#first-btn");
                this._first.addEventListener("click", this._onFirst);
                this._previous = document.querySelector("button#previous-btn");
                this._previous.addEventListener("click", this._onPrevious);
                this._next = document.querySelector("button#next-btn");
                this._next.addEventListener("click", this._onNext);
                this._last = document.querySelector("button#last-btn");
                this._last.addEventListener("click", this._onLast);
                window.addEventListener("popstate", this.transition);
            }
        }
        Nav.NavButtons = NavButtons;
    })(Nav = Shtik.Nav || (Shtik.Nav = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=nav.js.map