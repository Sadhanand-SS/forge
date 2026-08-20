import { Component } from '@angular/core';

@Component({
  selector: 'app-local-vault-status',
  standalone: true,
  template: `
    <div class="p-3.5 mx-3 mb-4 rounded-xl bg-[#141417] border border-zinc-800/80 shadow-sm flex items-center gap-3">
      <!-- Green Active Dot -->
      <div class="relative flex h-2.5 w-2.5">
        <span class="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span>
        <span class="relative inline-flex rounded-full h-2.5 w-2.5 bg-emerald-500"></span>
      </div>
      <div>
        <div class="text-[0.8rem] font-bold text-white tracking-wide">Local Vault</div>
        <div class="text-[0.7rem] text-zinc-500 font-medium">synced 2 min ago</div>
      </div>
    </div>
  `
})
export class LocalVaultStatusComponent {}
