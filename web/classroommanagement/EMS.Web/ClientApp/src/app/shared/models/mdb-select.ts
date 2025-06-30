export class MdbSelect {
    value: string;
    label: string;
    disabled: boolean;
    constructor(value: string, label: string, disabled: boolean = false) {
        this.value = value;
        this.label = label;
        this.disabled = disabled;
    }
}
