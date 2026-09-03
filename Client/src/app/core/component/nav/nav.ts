import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { UserService } from '../../services/user-service';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';

@Component({
  selector: 'app-nav',
  imports: [RouterLink, RouterLinkActive, ButtonModule, AvatarModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
constructor(public userService: UserService) {}
private router = inject(Router);
logout(){
  this.userService.logout();
  this.router.navigate(['/login']);
}
}
