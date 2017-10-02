/// <reference path="./notes.ts" />
/// <reference path="./questions.ts" />
/// <reference path="./nav.ts" />

// ReSharper disable once InconsistentNaming
interface LiveMessage {
    slideAvailable?: number;
}
declare namespace signalR {

    export class HubConnection {
        constructor(path: string);
        on(method: string, action: (data: LiveMessage) => void);
        invoke(method: string, data: any);
        start(): Promise<any>;
    }
}

namespace Shtik.AutoNav {
    import NotesForm = Notes.NotesForm;
    import QuestionsForm = Questions.QuestionsForm;
    import NavButtons = Nav.NavButtons;

    var notesForm: NotesForm;
    var questionsForm: QuestionsForm;
    var nav: NavButtons;
    var hubConnection: signalR.HubConnection;

    document.addEventListener("DOMContentLoaded", () => {

        notesForm = new NotesForm();
        notesForm.load();

        questionsForm = new QuestionsForm();
        questionsForm.load();

        nav = new NavButtons();

        hubConnection = new signalR.HubConnection("/realtime");
        hubConnection.on("Send",
            data => {
                if (data.slideAvailable) {
                    if (notesForm.dirty || questionsForm.dirty) return;
                    nav.go(window.location.pathname.replace(/\/[0-9]+$/, `/${data.slideAvailable}`));
                }
            });
        hubConnection.start()
            .then(() => {
                const groupName = window.location.pathname.replace(/\/[0-9]+$/, "");
                hubConnection.invoke("Join", groupName);
            });
    });
}