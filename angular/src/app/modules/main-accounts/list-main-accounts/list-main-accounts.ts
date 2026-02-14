import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MainAccountService } from 'src/app/proxy/app-services/main-accounts';
import { MainAccountDto, MainAccountPagedRequestDto } from 'src/app/proxy/dtos/main-accounts';
import { BilingualNamePipe } from 'src/app/shared/pipes/bilingual-name.pipe';
import { LocalizationPipe } from '@abp/ng.core';

@Component({
  selector: 'app-list-main-accounts',
  standalone: true,
  imports: [CommonModule, RouterLink, BilingualNamePipe, LocalizationPipe],
  templateUrl: './list-main-accounts.html',
  styleUrl: './list-main-accounts.scss'
})

export class ListMainAccounts implements OnInit {
  accounts: MainAccountDto[] = [];
  input: MainAccountPagedRequestDto = { maxResultCount: 10, skipCount: 0 };

  constructor(private mainAccountService: MainAccountService) {}

  ngOnInit(): void {
    this.loadMainAccounts();
  }

  loadMainAccounts(): void {
    this.mainAccountService.getList(this.input).subscribe(result => {
      this.accounts = result.items ?? [];
    });
  }

  trackById(_index: number, item: MainAccountDto): string {
    return item.id ?? '';
  }

  deleteAccount(account: MainAccountDto): void {
    if (!account?.id) return;
    if (!confirm('Are you sure you want to delete this account?')) return;
    this.mainAccountService.delete(account.id).subscribe({
      next: () => this.loadMainAccounts(),
      error: (err) => console.error(err)
    });
  }
}