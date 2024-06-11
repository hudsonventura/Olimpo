import React from 'react';

import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Tooltip from 'react-bootstrap/Tooltip';

function Tips(props) {

    
    const renderTooltip = () => (
        <Tooltip id="button-tooltip" >
            {props.message}
        </Tooltip>
    );
    
    return (
        <OverlayTrigger placement="right" delay={{ show: 250, hide: 400 }} overlay={renderTooltip('Add new stack')}>
            <a>
                {props.children}
            </a>
        </OverlayTrigger>
    ); 
}

export default Tips;