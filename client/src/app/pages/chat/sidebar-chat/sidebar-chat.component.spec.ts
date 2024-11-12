import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarChatComponent } from './sidebar-chat.component';

describe('SidebarChatComponent', () => {
  let component: SidebarChatComponent;
  let fixture: ComponentFixture<SidebarChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SidebarChatComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SidebarChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
