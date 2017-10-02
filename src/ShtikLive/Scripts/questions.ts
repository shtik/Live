namespace Shtik.Questions {

    interface IQuestion {
        id: string;
        user: string;
        text: string;
    }

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

    export class QuestionsForm {
        private _form: HTMLFormElement;
        private _textarea: HTMLTextAreaElement;
        private _button: HTMLButtonElement;
        private _list: HTMLUListElement;
        private _saving: boolean;
        private _autosaveTimeout: number;
        public dirty: boolean;

        constructor() {
            this._form = document.getElementById("questions") as HTMLFormElement;
            if (!!this._form) {

                this._textarea = this._form.querySelector("textarea") as HTMLTextAreaElement;
                this._button = this._form.querySelector("button") as HTMLButtonElement;
                this._list = this._form.querySelector("ul#question-list") as HTMLUListElement;
                window.addEventListener("popstate", this.load);
                this._form.addEventListener("submit", this._onSubmit);

                if (this._textarea) {
                    this._textarea.addEventListener("keyup", () => this.dirty = true);
                    this._textarea.addEventListener("paste", () => this.dirty = true);
                    this._textarea.addEventListener("focus", () => this.dirty = true);
                }
            }
        }

        public load = () => {
            if (!this._list) return;
            while (this._list.hasChildNodes()) {
                this._list.removeChild(this._list.lastChild);
            }
            fetch(this.questionsUrl, { method: "GET", credentials: "same-origin" })
                .then(r => r.text())
                .then(t => {
                    const questions = JSON.parse(t) as IQuestion[];
                    for (const q of questions) {
                        this._appendQuestion(q.id, q.user, q.text);
                    }
                });
        };

        public save = () => {
            if (!this._textarea || !this._setSaving(true)) return Promise.resolve(false);

            const question = this._textarea.value;
            const json = JSON.stringify({text: question});

            const headers = new Headers();
            headers.append("Content-Type", "application/json");
            headers.append("Content-Length", json.length.toString());

            return fetch(this.questionsUrl, { method: "POST", credentials: "same-origin", body: json, headers: headers })
                .then(_ => {
                    this._setSaving(false);
                    this.dirty = false;
                    return true;
                })
                .catch(r => {
                    console.log(r);
                    this._setSaving(false);
                    return false;
                });
        }

        public onMessage = (e: MessageEvent) => {
            const data = JSON.parse(e.data) as IMessage;
            if (data.MessageType === "question") {
                this._appendQuestion(data.Id, data.User, data.Text);
            }
        }

        private _appendQuestion = (id, user, text) => {
            id = `q${id}`;
            let li = this._list.querySelector(`#${id}`) as HTMLLIElement;
            if (!li) {
                li = document.createElement("li");
                li.id = id;
                li.className = "list-group-item";
                li.innerHTML =
                    `<span class="small"><strong>${user}</strong></span><br><span>${text}</span>`;
                this._list.appendChild(li);
            }
        }

        private _onSubmit = (e: Event) => {
            e.preventDefault();
            if (!!this._textarea.value) {
                this.save();
                this._textarea.value = null;
            }
        }

        private _setSaving = (value: boolean) => {
            if (this._saving && value) return false;
            this._saving = this._button.disabled = value;
            if (value) {
                this._button.classList.add("disabled");
            } else {
                this._button.classList.remove("disabled");
            }
            return this._saving;
        }

        private get questionsUrl() {
            const path = window.location.pathname.split("/");
            const slideNumber = path.pop();
            const slug = path.pop();
            const presenter = path.pop();
            return `/api/questions/${presenter}/${slug}/${slideNumber}`;
        }
    }


}