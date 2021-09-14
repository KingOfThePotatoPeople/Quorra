import {Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';
import {EventLog} from "../../shared/event-log";

@Injectable({
  providedIn: 'root'
})


export class EventService {

  endpoint: string = 'https://localhost:44378';

  headers = new HttpHeaders()
    .set('Content-Type', 'application/x-www-form-urlencoded')
    .set('Accept', 'application/json');

  constructor(private http: HttpClient) {
  }


  getEventLogs(): Observable<EventLog[]> {
    let api = `${this.endpoint}/api/log/auth`;
    return this.http.get<EventLog[]>(api, { headers: this.headers }).pipe(
      catchError(this.handleError)
    )
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
