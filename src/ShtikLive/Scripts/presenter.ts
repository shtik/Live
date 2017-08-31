namespace Shtik.Presenter {

// ReSharper disable InconsistentNaming
    interface IMessage {
        Id: string;
        MessageType: string;
        User: string;
        Slide: number;
        Time: string;
        Text: string;
    }
// ReSharper restore InconsistentNaming
    
    document.addEventListener("DOMContentLoaded", () => {

        const list = document.querySelector("ul#questions");

        const appendQuestion = (id, user, text) => {
            id = `q${id}`;
            let li = list.querySelector(`#${id}`) as HTMLLIElement;
            if (!li) {
                li = document.createElement("li");
                li.id = id;
                li.className = "list-group-item";
                li.innerHTML =
                    `<span class="small"><strong>${user}</strong></span><br><span>${text}</span>`;
                list.insertBefore(li, list.firstChild);
            }
        }

        const protocol = window.location.protocol === "https:" ? "wss:" : "ws:";
        const path = window.location.pathname.substr(8).replace(/\/[0-9]+$/, "");
        const wsUri = `${protocol}//${window.location.host}${path}`;
        const socket = new WebSocket(wsUri);

        socket.addEventListener("message", e => {
            const q = JSON.parse(e.data) as IMessage;
            if (q.MessageType === "question") {
                appendQuestion(q.Id, q.User, q.Text);
            }
        });

    });
}