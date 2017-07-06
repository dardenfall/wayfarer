(function($){
  'use strict';

  /////////// react stuff //////
  class App extends React.Component {
    constructor(props){
      super(props);
      var activeScenes = [props.scenes.scenes[0]];

      this.state = {
        currentScene:0,
        time:0,
        steps: 1000, 
        weather: props.scenes.getWeather(), 
        temp: props.scenes.getTemp(),
        calories: 2700,
        scenes:props.scenes.scenes,
        activeScenes:activeScenes};
    }

    choiceMade(nextScene, cost){

      const state = this.state;

      state.currentScene = nextScene;
      state.activeScenes.push(this.state.scenes[state.currentScene]);
      // update state
      this.setState({
        state
      });
    }

    addSteps(e){
      const state = this.state;

      state.steps += parseInt(document.querySelector("#debug-steps").value);
      this.setState({
        state
      });

    }

    render() {
      checkitem();

      return (
        <div>
          <div id="text-carousel" className="carousel slide" data-ride="carousel" data-wrap="false" interval="false">
            <div className="row">
              <div className="col-xs-offset-3 col-xs-6">
                <div className="carousel-inner">
                  {this.state.activeScenes.map( scene => 
                    <Scene
                     calories={this.state.calories} 
                     steps={this.state.steps}
                     weather={this.state.weather}
                     temp={this.state.temp}
                     time={this.state.time}
                     scene={scene}
                     currentScene={this.state.currentScene}
                     onChoiceMade={this.choiceMade.bind(this)}
                    />) 
                  }
                </div>
              </div>
            </div>
          </div>
          <div id='debug'>
            <h3>debug:</h3>
            <div className="input-group">
              <input id="debug-steps" type="text" className="form-control" placeholder="Number of steps" aria-describedby="basic-addon2"></input>
              <span className="input-group-addon" id="basic-addon2" onClick={this.addSteps.bind(this)}>add</span>
            </div>
          </div> 
        </div>
      )
    }
  }

  class Scene extends React.Component {

    constructor(props) {
      super(props);
    }

    getTime(){
      var times = ["early morning", "morning", "late morning", "noon", "afternoon", "late afternoon", "evening", "night", "midnight"];
    
      return times[this.props.time];
    }

    getSceneText(text) {
      return text.replace("<weather>", this.props.weather).
        replace("<temp>", this.props.temp).
        replace("<time>", this.getTime());
    }

    render(){

      let act = "item";
      console.log(this.props)
      if(this.props.currentScene === this.props.scene.id ){
        act = 'item active';
      }

      return (
        <div className={act}>
          <div className="carousel-content">
            <div className="card">
              <div className="card-header">
                <span className="step-count">Steps: {this.props.steps}</span>
                <span className="calorie-count">Calories: {this.props.calories}</span>
              </div>
              <div className="card-block">
                <p className="card-text">{this.getSceneText(this.props.scene.text)}</p>
                <br/>
                <ChoiceGroup choices={this.props.scene.choices} onChoiceMade={this.props.onChoiceMade} />
              </div>
            </div>
          </div>
        </div>
      )
    }
  }

  const ChoiceGroup = (props) => {

    return (
      <div className="list-group">
        {props.choices.map( choice => <Choice data={choice} onChoiceMade={props.onChoiceMade}/>) }
      </div>
    )
  }

  class Choice extends React.Component{
    constructor(props) {
      super(props);
    }

    handleClick(){
      console.log(this)
      this.props.onChoiceMade(this.props.data.nextScene,this.props.data.cost);
    }

    render() {
      return (
        <button type="button" onClick={this.handleClick.bind(this)} className="list-group-item" > {this.props.data.text} </button>
      )
    }
  }

  // 
  // DATA
  //

  const data = {
    scenes: [
      { 
        id: 0,
        text: "It is <time>.  The day is <weather> and <temp>. You are currently in the middle of a tall, green forest. You slept well last night.",
        choices:[
          {text:"Head down the trail further into the forest.", cost: null, nextScene:4 },
          {text:"Forage for food.", cost: null, nextScene:4},
          {text:"Use a skill", cost: null, nextScene:1}
        ]
      },
      { 
        id: 1,
        text: "Which skill do you want to use?",
        choices:[
          {text:"Stealth (current lvl 2.75)", cost: null, nextScene:2 },
          {text:"Meditation (current lvl 6.17)", cost: null, nextScene:2},
          {text:"Fishing (current lvl 1.32)", cost: null, nextScene:2},
          {text:"Swords (4.97)", cost: null, nextScene:2}
        ]
      },
      { 
        id: 2,
        text: "Do you want to?",
        choices:[
          {text:"Quickly cast a few times and hope you get a lucky strike", cost:250, nextScene:3},
          {text:"Keep fishing until you’ve caught a couple of fish", cost:500, nextScene:3},
          {text:"Collect some bait and search for the best place to cast", cost:1000, nextScene:3}
        ]
      },
      {
        id: 3,
        text: "You rig your fishing pole and head down to the nearby brook. You cast your rod for a hour or so, but nothing is biting. Dreaming of the one that got away, you head back to your camp.",
        choices:[
          {text:"Done", cost:null, nextScene:5}
        ]
      },
      {
        id: 4,
        text: "Hey Look, other choices work!!.",
        choices:[
          {text:"Goback", cost:null, nextScene:0}
        ]
      },
      {
        id: 5,
        text: "YOU WIN.",
        choices:[
        ]
      },            

    ],

    getWeather: function(){
      var weathers = ["sunny", "rainy", "cloudy", "snowy"]
      var random = Math.floor(Math.random() * (weathers.length));
      return weathers[random];
    },

    getTemp: function(){
      var temps = ["cold", "warm", "hot", "freezing"];
      var random = Math.floor(Math.random() * (temps.length));
      return temps[random];
    },
  }

  ReactDOM.render(<App scenes={data} />, document.getElementById('container'));


///////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////

  checkitem();
  $('#text-carousel').bind('slid.bs.carousel', function (e) {
      checkitem();
  });

  // document.querySelector("#add-card").addEventListener("click", function(){

  // $(".carousel-inner")
  //   .append('<div class="item active"> <div class="carousel-content"> <div class="card"> <div class="card-header"> Steps: <span class="step-count"></span> </div> <div class="card-block"> <h4 class="card-title">___NEW!!!___RIGHTMOST Special title treatment</h4> <p class="card-text">RIGHTMOST With supporting text below as a natural lead-in to additional content.</p> <a href="#" class="btn btn-primary">Go somewhere</a> </div> </div> </div> </div>');
  // });

  function checkitem(){

    if($('.carousel-content').length === 1){
      $('.left.carousel-control').hide();
      $('.right.carousel-control').hide();
    }else if($('.carousel-inner .item:first').hasClass('active')) {
      $('.left.carousel-control').hide();
      $('.right.carousel-control').show();
    } else if($('.carousel-inner .item:last').hasClass('active')) {
      $('.left.carousel-control').show();
      $('.right.carousel-control').hide();
    } else {
      $('.carousel-control').show();
    } 
  }


})(jQuery);

var headlines=document.querySelector('h2');
var stats=document.querySelector('.stat');
var result = [];
headlines.forEach(function(element, index){
  result[element.innerHTML] = stats[index];
})

['a','b','c']
['1','2','3']
[{a:'1'}, {b:'2'}, {c:'3'}]
