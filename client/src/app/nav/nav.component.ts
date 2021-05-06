import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {} //init object as any
  //loggedIn: boolean;
  currentUser$: Observable<User>;

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //this.getCurrentUser();
    //this.currentUser$ = this.accountService.currentUser$;
  }

  login() {
    console.log(this.model);
    this.accountService.login(this.model).subscribe(res => { // observable needs to be subscribe
      console.log(res);
      //this.loggedIn = true;
    }, error => {
      console.log(error);
    });
  }

  logout() {
    this.accountService.logout();
    //this.loggedIn = false;
  }



  // getCurrentUser() {
  //   this.accountService.current$.subscribe(user => {
  //     this.loggedIn = !!user; // cast an object into boolean. null into false
  //   }, error => {
  //     console.log(error);
  //   })
  // }
}
