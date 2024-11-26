import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserStateService {
  private stateFilter: string = 'All';
  constructor() { }

  getStateFilter() {
    return this.stateFilter;
  }

  setStateFilter(state: string) {
    this.stateFilter = state;
  }
}
