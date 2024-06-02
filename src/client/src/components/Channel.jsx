import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';
import { useState } from 'react';


import EditSensor from './EditStack';


function Channel({id, title, value, unit, type}) {

    const [showModalSensor, setShowModalSensor] = useState(false);
    
    const handleEditSensor = (id) => {
        console.log("Form enviado "+id);
        setShowModalSensor(true);
    }


    return (
        <>
            
            <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative' }}
                onDoubleClick={(e) => handleEditSensor(id)}
                >
                <a style={{ fontSize: '10px', position: 'relative', zIndex: 1, textAlign: 'left', marginTop: '5px' }}>
                    {title} {/* {(title.length > 30) ? title.substr(0, 35) + ' ...' : title} */}
                </a>
                <Badge style={{ position: 'absolute', right: '5px', bottom: '5px', zIndex: 2 }} bg={type}>
                    {value} {unit}
                </Badge>
            </Toast>
        </>
        



    ); 
}

export default Channel;