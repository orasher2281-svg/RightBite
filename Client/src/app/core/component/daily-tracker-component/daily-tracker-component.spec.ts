import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyTrackerComponent } from './daily-tracker-component';

describe('DailyTrackerComponent', () => {
  let component: DailyTrackerComponent;
  let fixture: ComponentFixture<DailyTrackerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DailyTrackerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(DailyTrackerComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
