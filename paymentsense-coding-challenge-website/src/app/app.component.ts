import { Component } from '@angular/core';
import { PaymentsenseCodingChallengeApiService } from './services';
import { CountryApiService } from './services/country-api.service';
import { CountriesComponent } from './countries/countries.component';
import { take } from 'rxjs/operators';
import { faThumbsUp, faThumbsDown } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public faThumbsUp = faThumbsUp;
  public faThumbsDown = faThumbsDown;
  public title = 'Paymentsense Coding Challenge!';
  public paymentsenseCodingChallengeApiIsActive = false;
  public paymentsenseCodingChallengeApiActiveIcon = this.faThumbsDown;
  public paymentsenseCodingChallengeApiActiveIconColour = 'red';
  public countries;

  constructor(private paymentsenseCodingChallengeApiService: PaymentsenseCodingChallengeApiService,
              private countryApiService: CountryApiService) {
    paymentsenseCodingChallengeApiService.getHealth().pipe(take(1))
      .subscribe(
        apiHealth => {
          this.paymentsenseCodingChallengeApiIsActive = apiHealth === 'Healthy';
          this.paymentsenseCodingChallengeApiActiveIcon = this.paymentsenseCodingChallengeApiIsActive
            ? this.faThumbsUp
            : this.faThumbsUp;
          this.paymentsenseCodingChallengeApiActiveIconColour = this.paymentsenseCodingChallengeApiIsActive
            ? 'green'
            : 'red';
        },
        _ => {
          this.paymentsenseCodingChallengeApiIsActive = false;
          this.paymentsenseCodingChallengeApiActiveIcon = this.faThumbsDown;
          this.paymentsenseCodingChallengeApiActiveIconColour = 'red';
        });
    countryApiService.getCountries().subscribe(countries => {
      console.log(countries);
      this.countries = countries;
    });
  }
}
