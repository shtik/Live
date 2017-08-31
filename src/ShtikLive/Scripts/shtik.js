/// <reference path="./notes.ts" />
/// <reference path="./questions.ts" />
/// <reference path="./nav.ts" />
var Shtik;
(function (Shtik) {
    var AutoNav;
    (function (AutoNav) {
        var NotesForm = Shtik.Notes.NotesForm;
        var QuestionsForm = Shtik.Questions.QuestionsForm;
        var NavButtons = Shtik.Nav.NavButtons;
        // ReSharper restore InconsistentNaming
        var notesForm;
        var questionsForm;
        var nav;
        document.addEventListener("DOMContentLoaded", () => {
            notesForm = new NotesForm();
            notesForm.load();
            questionsForm = new QuestionsForm();
            questionsForm.load();
            nav = new NavButtons();
            const protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
            const path = window.location.pathname.substr(5).replace(/\/[0-9]+$/, "");
            const wsUri = `${protocol}//${window.location.host}${path}`;
            const socket = new WebSocket(wsUri);
            socket.addEventListener("message", e => {
                const data = JSON.parse(e.data);
                if (data.MessageType === "slideshown") {
                    if (notesForm.dirty || questionsForm.dirty)
                        return;
                    nav.go(window.location.pathname.replace(/\/[0-9]+$/, `/${data.Slide}`));
                }
            });
            socket.addEventListener("message", questionsForm.onMessage);
        });
        console.log("Wibble");
    })(AutoNav = Shtik.AutoNav || (Shtik.AutoNav = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=shtik.js.map