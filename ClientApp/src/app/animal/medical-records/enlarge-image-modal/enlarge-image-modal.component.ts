import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-enlarge-image-modal',
  templateUrl: './enlarge-image-modal.component.html',
  styleUrls: ['./enlarge-image-modal.component.css']
})
export class EnlargeImageModalComponent implements OnInit {

  @Input() imagePath;

  constructor(private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
  }

}
