var Shtik;
(function (Shtik) {
    var Questions;
    (function (Questions) {
        // ReSharper restore InconsistentNaming
        class QuestionsForm {
            constructor() {
                this.load = () => {
                    while (this._list.hasChildNodes()) {
                        this._list.removeChild(this._list.lastChild);
                    }
                    fetch(this.questionsUrl, { method: "GET", credentials: "same-origin" })
                        .then(r => r.text())
                        .then(t => {
                        const questions = JSON.parse(t);
                        for (const q of questions) {
                            this._appendQuestion(q.id, q.user, q.text);
                        }
                    });
                };
                this.save = () => {
                    if (!this._setSaving(true))
                        return Promise.resolve(false);
                    const question = this._textarea.value;
                    const json = JSON.stringify({ text: question });
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
                };
                this.onMessage = (e) => {
                    const data = JSON.parse(e.data);
                    if (data.MessageType === "question") {
                        this._appendQuestion(data.Id, data.User, data.Text);
                    }
                };
                this._appendQuestion = (id, user, text) => {
                    id = `q${id}`;
                    let li = this._list.querySelector(`#${id}`);
                    if (!li) {
                        li = document.createElement("li");
                        li.id = id;
                        li.className = "list-group-item";
                        li.innerHTML =
                            `<span class="small"><strong>${user}</strong></span><br><span>${text}</span>`;
                        this._list.appendChild(li);
                    }
                };
                this._onSubmit = (e) => {
                    e.preventDefault();
                    if (!!this._textarea.value) {
                        this.save();
                        this._textarea.value = null;
                    }
                };
                this._setSaving = (value) => {
                    if (this._saving && value)
                        return false;
                    this._saving = this._button.disabled = value;
                    if (value) {
                        this._button.classList.add("disabled");
                    }
                    else {
                        this._button.classList.remove("disabled");
                    }
                    return this._saving;
                };
                this._form = document.getElementById("questions");
                this._textarea = this._form.querySelector("textarea");
                this._button = this._form.querySelector("button");
                this._list = this._form.querySelector("ul#question-list");
                window.addEventListener("popstate", this.load);
                this._form.addEventListener("submit", this._onSubmit);
                this._textarea.addEventListener("keyup", () => this.dirty = true);
                this._textarea.addEventListener("paste", () => this.dirty = true);
                this._textarea.addEventListener("focus", () => this.dirty = true);
            }
            get questionsUrl() {
                const path = window.location.pathname.split("/");
                const slideNumber = path.pop();
                const slug = path.pop();
                const presenter = path.pop();
                return `/api/questions/${presenter}/${slug}/${slideNumber}`;
            }
        }
        Questions.QuestionsForm = QuestionsForm;
    })(Questions = Shtik.Questions || (Shtik.Questions = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=questions.js.map