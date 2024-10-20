import { Injectable } from '@angular/core';
import { AppSetting, default_app } from '../../../config';
import { BehaviorSubject, Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AppConfigService {
  get notify(): Observable<Record<string, any>> {
    return this.notify$.asObservable();
  }
  private notify$ = new BehaviorSubject<Record<string, any>>({});

  constructor() {

  }

  getAppSetting(): AppSetting {
    var appSetting;
    try {
      if (localStorage.getItem('option_setting_left_sidebar')) {
        appSetting = JSON.parse(localStorage.getItem('option_setting_left_sidebar') as string) || null;
      }
    }
    catch (ex) {
      console.error(ex);
    }
    return appSetting || default_app ;
  }

  setAppSetting(options: AppSetting) {
    this.options = Object.assign(default_app, options);
    this.notify$.next(this.options);
  }

  private options = this.getAppSetting();

}
