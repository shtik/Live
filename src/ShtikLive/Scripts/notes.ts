namespace Shtik.Notes {
    
    interface INote {
        text: string;
    }

    export class NotesForm {
        private _form: HTMLFormElement;
        private _textarea: HTMLTextAreaElement;
        private _button: HTMLButtonElement;
        private _saving: boolean;
        private _autosaveTimeout: number;
        public dirty: boolean;

        constructor() {
            this._form = document.getElementById("notes") as HTMLFormElement;
            this._textarea = this._form.querySelector("textarea") as HTMLTextAreaElement;
            this._button = this._form.querySelector("button") as HTMLButtonElement;
            window.addEventListener("popstate", this.load);
            this._textarea.addEventListener("keyup", this._autoSave);
            this._textarea.addEventListener("paste", this._autoSave);
            this._textarea.addEventListener("focus", () => this.dirty = true);
            this._textarea.addEventListener("blur", () => this.dirty = false);
        }

        private _autoSave = () => {
            if (this._autosaveTimeout) {
                clearTimeout(this._autosaveTimeout);
            }
            this._autosaveTimeout = setTimeout(() => {
                this.save()
                    .then(saved => {
                        if (!saved) {
                            this._autoSave();
                        }
                    });
            }, 2000);
        }

        public load = () => {
            fetch(this.notesUrl, { method: "GET", credentials: "same-origin" })
                .then(r => r.text())
                .then(t => {
                    const note = JSON.parse(t) as INote;
                    this._textarea.value = note.text;
                });
        };

        public save = () => {
            if (!this._setSaving(true)) return Promise.resolve(false);

            const notes = this._textarea.value;
            const json = JSON.stringify({text: notes});

            const headers = new Headers();
            headers.append("Content-Type", "application/json");
            headers.append("Content-Length", json.length.toString());

            return fetch(this.notesUrl, { method: "PUT", credentials: "same-origin", body: json, headers: headers })
                .then(r => {
                    this._setSaving(false);
                    return true;
                })
                .catch(r => {
                    console.log(r);
                    this._setSaving(false);
                    return false;
                });
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

        private get notesUrl() {
            const path = window.location.pathname.split("/");
            const slideNumber = path.pop();
            const slug = path.pop();
            const presenter = path.pop();
            return `/api/notes/${presenter}/${slug}/${slideNumber}`;
        }
    }
}