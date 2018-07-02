import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware } from 'redux';
import thunk from 'redux-thunk';
import { composeWithDevTools } from 'redux-devtools-extension';

import {
  BrowserRouter as Router,
  Route,
  Link,
  Switch
} from 'react-router-dom'

import allReducers from './reducers';

import App from './components/App/App';
import NotFound from './components/App/NotFound';

import './styles/styles.scss';

const store = createStore(
  allReducers,
  {}, 
  applyMiddleware(...[thunk]),
  window.devToolsExtension && window.devToolsExtension()
);

render((
  <Provider store={store}>
    <Router>
      <App>
        <Switch>
          <Route exact path="/" component={NotFound}/>
          {/* <Route component={NotFound}/> */}
        </Switch>
      </App>
    </Router>
  </Provider>
), document.getElementById('app'));
