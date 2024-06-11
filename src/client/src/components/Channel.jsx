import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';
import { useState } from 'react';


import EditSensor from './EditStack';
import Tips from '../components/Tips';


function Channel({channel}) {

    const [showModalSensor, setShowModalSensor] = useState(false);
    
    const handleEditSensor = () => {
        console.log("Form enviado "+channel.id);
        setShowModalSensor(true);
    }

    let type = (channel.current_metric.error_code > 0) ? 'danger' : 'success';

    return (
        <>
            <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative' }}
                onDoubleClick={(e) => handleEditSensor()}
                >
                    <div className={"text-bg-"+type} style={{borderStyle: "solid", position: "absolute", width: "9px", height: "90%", marginLeft: "-11px", marginTop: "3px",  borderRadius: "6px"}}></div>
                    <Tips message={(channel.current_metric.error_code > 0) ? channel.current_metric.message : 'Success'}>
                    <a style={{ fontSize: '10px', position: 'relative', zIndex: 1, textAlign: 'left', marginTop: '5px' }}>
                        {channel.name} {channel.error} {/* {(title.length > 30) ? title.substr(0, 35) + ' ...' : title} */}
                    </a>
                    <Badge style={{ position: 'absolute', right: '5px', bottom: '5px', zIndex: 2 }} bg={type}>
                        {channel.current_metric.value} {channel.unit}
                    </Badge>
                    </Tips>
            </Toast>
        </>
        



    ); 
}

export default Channel;