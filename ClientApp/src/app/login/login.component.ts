import {Component, OnInit} from '@angular/core';
import {FormGroup, FormControl} from '@angular/forms';
import { AuthService } from '../services/auth-service/auth-service';
import { User } from '../shared/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService) {
  }

  signInForm = new FormGroup({
    email: new FormControl(''),
    password: new FormControl('')
  });

  user = new User()

  ngOnInit(): void {
  }

  onSubmit() {

    this.user.email = this.signInForm.value.email;
    this.user.password = this.signInForm.value.password;

    if (this.signInForm.value.email && this.signInForm.value.password) {
      this.authService.signIn(this.user);
    }
  }

}
