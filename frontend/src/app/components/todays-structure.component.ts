import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MealSlotComponent } from './meal-slot.component';

@Component({
  selector: 'app-todays-structure',
  standalone: true,
  imports: [CommonModule, MealSlotComponent],
  template: `
    <div class="p-6 rounded-2xl bg-[#17171a] border border-zinc-800 flex flex-col justify-between h-full shadow-sm hover:border-zinc-800/80 transition-all select-none">
      <!-- Card Header -->
      <div class="mb-6">
        <h3 class="text-[0.75rem] font-bold text-zinc-500 tracking-widest uppercase font-mono mb-1">
          Today's Structure
        </h3>
        <p class="text-xs text-zinc-400 font-medium">
          {{ slots().length }} meal slots
        </p>
      </div>

      <!-- Meal Slots List -->
      <div class="flex flex-col gap-2.5">
        @for (slot of slots(); track slot.index) {
          <app-meal-slot
            [index]="slot.index"
            [title]="slot.title"
            [time]="slot.time"
          ></app-meal-slot>
        }
      </div>
    </div>
  `
})
export class TodaysStructureComponent {
  slots = input<Array<{ index: string; title: string; time: string }>>([
    { index: '01', title: 'Breakfast', time: '08:00' },
    { index: '02', title: 'Lunch', time: '13:00' },
    { index: '03', title: 'Dinner', time: '20:00' }
  ]);
}
