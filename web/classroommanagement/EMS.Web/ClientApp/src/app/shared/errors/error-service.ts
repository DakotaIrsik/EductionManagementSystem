import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class ErrorService {

    public error: HttpErrorResponse;

    constructor(private router: Router) { }

    public handleError(error: HttpErrorResponse) {
        this.error = error;
        this.router.navigate(['/error']);
    }
}
