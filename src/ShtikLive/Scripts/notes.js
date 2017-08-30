var Shtik;
(function (Shtik) {
    var Notes;
    (function (Notes) {
        class NotesForm {
            constructor() {
                this._autoSave = () => {
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
                };
                this.load = () => {
                    fetch(this.notesUrl, { method: "GET", credentials: "same-origin" })
                        .then(r => r.text())
                        .then(t => {
                        const note = JSON.parse(t);
                        this._textarea.value = note.text;
                    });
                };
                this.save = () => {
                    if (!this._setSaving(true))
                        return Promise.resolve(false);
                    const notes = this._textarea.value;
                    const json = JSON.stringify({ text: notes });
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
                this._form = document.getElementById("notes");
                this._textarea = this._form.querySelector("textarea");
                this._button = this._form.querySelector("button");
                window.addEventListener("popstate", this.load);
                this._textarea.addEventListener("keyup", this._autoSave);
                this._textarea.addEventListener("paste", this._autoSave);
            }
            get notesUrl() {
                const path = window.location.pathname.split("/");
                const slideNumber = path.pop();
                const slug = path.pop();
                const presenter = path.pop();
                return `/api/notes/${presenter}/${slug}/${slideNumber}`;
            }
        }
        Notes.NotesForm = NotesForm;
    })(Notes = Shtik.Notes || (Shtik.Notes = {}));
})(Shtik || (Shtik = {}));
//# sourceMappingURL=notes.js.map