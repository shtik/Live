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
        var notesForm;
        var questionsForm;
        var nav;
        var hubConnection;
        document.addEventListener("DOMContentLoaded", () => {
            notesForm = new NotesForm();
            notesForm.load();
            questionsForm = new QuestionsForm();
            questionsForm.load();
            nav = new NavButtons();
            hubConnection = new signalR.HubConnection("/realtime");
            hubConnection.on("Send", data => {
                if (data.slideAvailable) {
                    if (notesForm.dirty || questionsForm.dirty)
                        return;
                    nav.go(window.location.pathname.replace(/\/[0-9]+$/, `/${data.slideAvailable}`));
                }
            });
            hubConnection.start()
                .then(() => {
                const groupName = window.location.pathname.replace(/\/[0-9]+$/, "");
                hubConnection.invoke("Join", groupName);
            });
        });
    })(AutoNav = Shtik.AutoNav || (Shtik.AutoNav = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=shtik.js.map