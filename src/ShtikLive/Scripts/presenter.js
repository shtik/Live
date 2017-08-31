var Shtik;
(function (Shtik) {
    var Presenter;
    (function (Presenter) {
        // ReSharper restore InconsistentNaming
        document.addEventListener("DOMContentLoaded", () => {
            const list = document.querySelector("ul#questions");
            const appendQuestion = (id, user, text) => {
                id = `q${id}`;
                let li = list.querySelector(`#${id}`);
                if (!li) {
                    li = document.createElement("li");
                    li.id = id;
                    li.className = "list-group-item";
                    li.innerHTML =
                        `<span class="small"><strong>${user}</strong></span><br><span>${text}</span>`;
                    list.insertBefore(li, list.firstChild);
                }
            };
            const protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
            const path = window.location.pathname.substr(8).replace(/\/[0-9]+$/, "");
            const wsUri = `${protocol}//${window.location.host}${path}`;
            const socket = new WebSocket(wsUri);
            socket.addEventListener("message", e => {
                const q = JSON.parse(e.data);
                if (q.MessageType === "question") {
                    appendQuestion(q.Id, q.User, q.Text);
                }
            });
        });
    })(Presenter = Shtik.Presenter || (Shtik.Presenter = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=presenter.js.map