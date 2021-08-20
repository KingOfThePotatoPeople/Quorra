import {Injectable} from "@angular/core";
import {User} from '../../shared/user';
import {AuthModel} from '../../shared/auth-model';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  endpoint: string = 'https://localhost:44378';

  headers = new HttpHeaders()
    .set('Content-Type', 'application/x-www-form-urlencoded')
    .set('Accept', 'application/json');
  currentUser = {};
  authModel = new AuthModel();

  constructor(private http: HttpClient,
              public router: Router
  ) {
  }

  // Sign-up
  signUp(user: User): Observable<any> {
    let api = `${this.endpoint}/register-user`;
    return this.http.post(api, user)
      .pipe(
        catchError(this.handleError)
      )
  }

  // Set http options
  setHttpOptions(authModel: AuthModel) {

    return new HttpParams()
        .set('grant_type', authModel.grant_type)
        .set('username', authModel.username)
        .set('password', authModel.password)
        .set('client_id', authModel.client_id)
        .set('client_secret', authModel.client_secret)
        .set('scope', authModel.scope)
    ;

  }

  // Sign-in
  signIn(user: User) {
    this.authModel.username = user.email;
    this.authModel.password = user.password;

    console.log(this.authModel);

    return this.http.post<AuthModel>(`${this.endpoint}/connect/token`, this.setHttpOptions(this.authModel), {headers: this.headers})
      .subscribe((res: any) => {
        localStorage.setItem('access_token', res.access_token)
        this.router.navigate(['dash']);
      })
  }

  // User profile
  getUserProfile(id): Observable<any> {
    let api = `${this.endpoint}/user-profile/${id}`;
    return this.http.get(api, { headers: this.headers }).pipe(
      map((res: Response) => {
        return res || {}
      }),
      catchError(this.handleError)
    )
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  get isLoggedIn(): boolean {
    let authToken = localStorage.getItem('access_token');
    return (authToken !== null);
  }

  doLogout() {
    let removeToken = localStorage.removeItem('access_token');
    if (removeToken == null) {
      this.router.navigate(['login']);
    }
  }

  // Error
  handleError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      msg = error.error.message;
    } else {
      // server-side error
      msg = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(msg);
  }

}
