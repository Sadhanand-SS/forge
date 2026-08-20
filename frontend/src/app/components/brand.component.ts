import { Component } from '@angular/core';

@Component({
  selector: 'app-brand',
  standalone: true,
  template: `
    <div class="flex items-center gap-3 px-6 py-5 border-b border-zinc-800/60">
      <!-- Flame Icon inside Rounded Box -->
      <div class="flex items-center justify-center w-10 h-10 rounded-lg bg-[#18181c] border border-zinc-800 shadow-inner">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-5 h-5 text-[#ea580c]">
          <path d="M8.5 14.5A2.5 2.5 0 0 0 11 12c0-1.38-.5-2-1-3-1.072-2.143-.224-4.054 2-6 .5 2.5 2 4.9 4 6.5 2 1.6 3 3.5 3 5.5a7 7 0 1 1-14 0c0-1.153.433-2.294 1-3a2.5 2.5 0 0 0 2.5 2.5z" />
        </svg>
      </div>
      <div>
        <div class="text-[1.05rem] font-bold text-white tracking-widest uppercase font-mono">Forge</div>
        <div class="text-[0.7rem] text-zinc-500 font-medium">Diet Dashboard</div>
      </div>
    </div>
  `
})
export class BrandComponent {}
