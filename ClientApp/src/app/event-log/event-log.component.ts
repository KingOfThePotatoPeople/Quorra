import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event-service/event-service';
import { EventLog } from "../shared/event-log";

@Component({
  selector: 'app-event-log',
  templateUrl: './event-log.component.html',
  styleUrls: ['./event-log.component.css']
})
export class EventLogComponent implements OnInit {

  logs: EventLog[];

  constructor(public eventService: EventService) {
  }

  ngOnInit(): void {

    this.eventService.getEventLogs().subscribe(res => {
      this.logs = res;
      console.log(res);
    });


  }

}
