/// <reference path="./notes.ts" />
var Shtik;
(function (Shtik) {
    var AutoNav;
    (function (AutoNav) {
        var NotesForm = Shtik.Notes.NotesForm;
        function loadSlide(url) {
            return fetch(url, { method: "GET" })
                .then(response => response.text());
        }
        function transition() {
            const url = window.location.href + "/partial";
            loadSlide(url)
                .then(json => {
                const partial = JSON.parse(json);
                document.querySelector("div.slide").innerHTML = partial.slide.html;
            });
        }
        function go(href) {
            history.pushState(null, null, href);
            transition();
        }
        var notesForm;
        document.addEventListener("DOMContentLoaded", () => {
            const protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
            const path = window.location.pathname.substr(5).replace(/\/[0-9]+$/, "");
            const wsUri = `${protocol}//${window.location.host}${path}`;
            const socket = new WebSocket(wsUri);
            socket.onmessage = e => {
                const data = JSON.parse(e.data);
                if (data.slide) {
                    go(window.location.pathname.replace(/\/[0-9]+$/, `${data.slide}`));
                }
            };
            window.addEventListener("popstate", transition);
            notesForm = new NotesForm();
            notesForm.load();
        });
        console.log("Bobbins");
    })(AutoNav = Shtik.AutoNav || (Shtik.AutoNav = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=shtik.js.map