import Toast from 'react-bootstrap/Toast';
import Badge from 'react-bootstrap/Badge';

import {useNavigate} from 'react-router-dom';



function Channel({id, title, value, unit, type}) {

    
    const navigate = useNavigate();
    const handleChannel = (id) => {
        console.log('2 cliques no channel id '+id);
        navigate('/sensor/'+id);
    }


    return (
        <Toast style={{ marginLeft: '12px', marginRight: '-9px', width: '180px', height: '45px', display: 'flex', flexDirection: 'column', justifyContent: 'flex-start', paddingRight: '50px', position: 'relative' }}
            onDoubleClick={(e) => handleChannel(id)}
            >
            <a style={{ fontSize: '10px', position: 'relative', zIndex: 1, textAlign: 'left', marginTop: '5px' }}>
                {title} {/* {(title.length > 30) ? title.substr(0, 35) + ' ...' : title} */}
            </a>
            <Badge style={{ position: 'absolute', right: '5px', bottom: '5px', zIndex: 2 }} bg={type}>
                {value} {unit}
            </Badge>
        </Toast>



    ); 
}

export default Channel;