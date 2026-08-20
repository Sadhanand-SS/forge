import { Component, input } from '@angular/core';

@Component({
  selector: 'app-meal-slot',
  standalone: true,
  template: `
    <div class="flex items-center justify-between p-3.5 rounded-xl bg-[#121215] border border-zinc-800/80 shadow-sm hover:border-zinc-800 transition-colors select-none">
      <div class="flex items-center gap-3">
        <!-- Number indicator badge -->
        <div class="flex items-center justify-center px-2 py-0.5 rounded bg-[#18181c] border border-zinc-800/60 text-[0.65rem] font-bold text-zinc-500 font-mono">
          {{ index() }}
        </div>
        <span class="text-[0.875rem] font-bold text-white tracking-wide">
          {{ title() }}
        </span>
      </div>
      <span class="text-xs text-zinc-500 font-medium font-mono">
        {{ time() }}
      </span>
    </div>
  `
})
export class MealSlotComponent {
  index = input.required<string | number>();
  title = input.required<string>();
  time = input.required<string>();
}
