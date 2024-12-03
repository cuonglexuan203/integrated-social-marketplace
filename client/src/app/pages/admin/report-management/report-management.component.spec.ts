import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportManagementComponent } from './report-management.component';

describe('ReportManagementComponent', () => {
  let component: ReportManagementComponent;
  let fixture: ComponentFixture<ReportManagementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReportManagementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
