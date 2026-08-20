import { Component, input } from '@angular/core';
import { NutritionSummaryComponent } from './nutrition-summary.component';

@Component({
  selector: 'app-daily-output-card',
  standalone: true,
  imports: [NutritionSummaryComponent],
  template: `
    <div class="p-6 rounded-2xl bg-[#17171a] border border-zinc-800 flex flex-col justify-between h-full shadow-sm hover:border-zinc-800/80 transition-all select-none">
      <!-- Card Header -->
      <div class="mb-6">
        <h3 class="text-[0.75rem] font-bold text-zinc-500 tracking-widest uppercase font-mono mb-1">
          Daily Output
        </h3>
        <p class="text-xs text-zinc-400 font-medium">
          Sum of all configured meals
        </p>
      </div>

      <!-- Main Content (Calories) -->
      <div class="flex items-baseline gap-2 mb-8">
        <span class="text-[3.75rem] font-bold text-white tracking-tight leading-none">
          {{ calories() }}
        </span>
        <span class="text-sm font-medium text-zinc-500 font-mono">
          kcal / day
        </span>
      </div>

      <!-- Bottom Nutrition Summary Row -->
      <app-nutrition-summary
        [energy]="calories()"
        [protein]="protein()"
        [carbs]="carbs()"
        [fat]="fat()"
        [fiber]="fiber()"
      ></app-nutrition-summary>
    </div>
  `
})
export class DailyOutputCardComponent {
  calories = input<number>(2444);
  protein = input<number>(185);
  carbs = input<number>(242);
  fat = input<number>(76);
  fiber = input<number>(21);
}
