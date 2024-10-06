import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class CancelRequestService {
    private cancelPendingRequests$ = new Subject<void>();

    constructor() { }

    public cancelPendingRequests(): void {
        this.cancelPendingRequests$.next();
    }

    public onCancelPendingRequests() {
        return this.cancelPendingRequests$.asObservable();
    }
}