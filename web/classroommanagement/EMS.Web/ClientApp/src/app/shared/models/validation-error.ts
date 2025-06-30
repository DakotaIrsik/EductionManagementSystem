export class ValidationError {
    public message: string;
    public value: string;
    constructor(value: string, message: string) {
        this.value = value;
        this.message = message;
    }
}