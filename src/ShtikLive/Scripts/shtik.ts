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
        on(method: string, action: (data: LiveMessage) => void): void;
        onclose(callback: ConnectionClosed): void;
        invoke(method: string, data: any);
        start(): Promise<any>;
    }
    export type ConnectionClosed = (e?: Error) => void;
}

namespace Shtik.AutoNav {
    import NotesForm = Notes.NotesForm;
    import QuestionsForm = Questions.QuestionsForm;
    import NavButtons = Nav.NavButtons;

    var notesForm: NotesForm;
    var questionsForm: QuestionsForm;
    var nav: NavButtons;
    var hubConnection: signalR.HubConnection;

    function hubConnect() {
        hubConnection = new signalR.HubConnection("/realtime");
        hubConnection.on("Send",
            data => {
                if (data.slideAvailable) {
                    if (notesForm.dirty || questionsForm.dirty) return;
                    nav.go(window.location.pathname.replace(/\/[0-9]+$/, `/${data.slideAvailable}`));
                }
            });
        hubConnection.onclose(e => {
            console.error(e.message);
            setTimeout(hubConnect, 1000);
        });
        hubConnection.start()
            .then(() => {
                const groupName = window.location.pathname.replace("/live/", "").replace(/\/[0-9]+$/, "");
                hubConnection.invoke("Join", groupName);
            });
    }

    document.addEventListener("DOMContentLoaded", () => {

        notesForm = new NotesForm();
        notesForm.load();

        questionsForm = new QuestionsForm();
        questionsForm.load();

        nav = new NavButtons();

        hubConnect();
    });
}