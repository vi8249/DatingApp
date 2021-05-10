import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  registerForm: FormGroup;
  maxDate: Date;

  //store validation errors coming back from server. 
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private toastr: ToastrService
    , private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      //form control
      gender: ["male"],
      username: ["", Validators.required],
      knownAs: ["", Validators.required],
      dateOfBirth: ["", Validators.required],
      city: ["", Validators.required],
      Country: ["", Validators.required],
      password: ["", [Validators.required
        , Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ["", [Validators.required, this.matchValues("password")]]
    });
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  //custom validator
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      // return null if confirmPasswrod matches password
      // isMatching is for validation notifly from html
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

  register() {
    //console.log(this.registerForm.value);

    this.accountService.register(this.registerForm.value).subscribe(res => {
      this.router.navigateByUrl("/members")
      this.cancel();
    }, error => { // http error response
      this.validationErrors = error;

    })
  }

  cancel() {
    console.log("cancelled");
    this.cancelRegister.emit(false);
  }

}
