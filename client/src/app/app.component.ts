import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'My dating app';
  users: any;

  constructor(private accountService: AccountService) {
  }

  ngOnInit(): void {
    //this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser() {
    // fetch user data from local storage
    // set as current user
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }

  // getUsers() {
  //   this.http.get('https://localhost:5001/api/users').subscribe(res => {
  //     this.users = res;
  //   }, error => {
  //     console.log((error));
  //   });
  // }

}
